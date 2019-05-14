/**
 * Created by Deepak on 5/9/2016.
 */

(function ($) {

	$(document).ready(function () {

		var pageItemClass = "dflip-page-item",
			pageEmptyItemClass = "dflip-empty-page",
			pageThumbClass = "dflip-page-thumb",
			activeClass = "dflip-active",
			hashUpdateClass = "dflip-update-hash",
			pageOptionsClass = "dflip-page-options",
			pageList = $("#dflip_page_list"),
			pageListBox = $("#dflip_pages_box"),
			outlineBox = $("#dflip_outline_box"),
			uploadMediaClass = "dflip_upload_media",
			tabsId = "dflip-tabs",
			tabsListId = "dflip-tabs-list";

		$(document).on("click", "." + tabsListId + " a", function (event) {
			event.preventDefault();

			var parent = $(this).parent();

			if (parent.hasClass(activeClass)) return;

			var target_id = $(this).attr("href").replace("!", "");
			var target = $(this).closest("." + tabsId).find(target_id);

			var tab = (parent[0].nodeName == "LI") ? parent : $(this);
			var tabActiveClass = activeClass;
			if (tab.hasClass("nav-tab"))
				tabActiveClass += " nav-tab-active";

			tab.siblings().removeClass(tabActiveClass);
			tab.addClass(tabActiveClass);

			target.siblings().removeClass(tabActiveClass);
			target.addClass(tabActiveClass);

			if (parent.hasClass(hashUpdateClass)) {
				var hash = this.hash.split('#').join('#!');
				window.location.hash = hash;
				updatePostHash(hash);
			}
		});

		function updatePostHash(value) {
			var post_link = $('#post').attr('action');
			if (post_link) {
				post_link = post_link.split('#')[0];
				$('#post').attr('action', post_link + value);
			}
		}

		if (window.location.hash && window.location.hash.indexOf('!dflip-tab-') >= 0) {
			$('.' + tabsListId).find('a[href="' + window.location.hash.replace('!', '') + '"]').trigger("click");
			updatePostHash(window.location.hash);
		}

		// Enable page sort
		if (pageList.length > 0) {
			pageList.sortable({
				containment: pageListBox,
				items: "> ." + pageItemClass
			});
			var newPageIndex = pageList.find("." + pageItemClass).length;
			pageList.find("." + pageItemClass).each(function (index) {
				$(this).attr("index", index);
			});
			pageList.append(newPageItem({}, newPageIndex));
		}
		newPageIndex++;
		function uploadMedia(options) {
			var title = options.title || 'Select File',
				text = options.text || 'Send to dFlip',
				urlInput = options.target;

			var multiple = options.multiple == true ? 'add' : false;
			var uploader = wp.media({
				multiple: multiple,
				title: title,
				button: {
					text: text
				},
				library: {
					type: options.type
				}

			})
				.on('select', function () {
					var files = uploader.state().get('selection');

					if (multiple == false) {
						var fileUrl = files.models[0].attributes.url;
						urlInput.val(fileUrl);
						if (options.callback) options.callback(fileUrl);
					} else {
						if (options.callback) options.callback(files);
					}


				})
				.open();
		}

		//upload doc
		$(document).on('click', '#dflip_upload_pdf_source', function (e) {
			e.preventDefault();
			uploadMedia({
				target: $(this).parent().find("input"),
				type: 'application/pdf'
			});
		});

		$(document).on('click', '#dflip_upload_pdf_thumb,#dflip_upload_bg_image', function (e) {
			e.preventDefault();
			uploadMedia({
				target: $(this).parent().find("input"),
				type: 'image'
			});
		});

		$(document).on('click', '.dflip-page-list-add', function (e) {
			e.preventDefault();
			var pageItem = pageList.find("." + pageEmptyItemClass);
			uploadMedia({
				target: pageItem.find("input"),
				type: 'image',
				multiple: true,
				callback: function (files) {
					for (var fileCount = 0; fileCount < files.length; fileCount++) {

						pageItem = pageList.find("." + pageEmptyItemClass).removeClass(pageEmptyItemClass).addClass(pageItemClass);
						var fileUrl = files.models[fileCount].attributes.url;
						pageItem.find("input").val(fileUrl);
						pageItem.find("." + pageThumbClass).attr("src", fileUrl);

						pageList.append(newPageItem({}, newPageIndex));
						newPageIndex++;
					}
				}
			});
		});


		function newPageItem(options, index) {

			var src = options.src || '',
				title = options.title || '',
				content = options.content || '',
				hotspot = options.hotspot || '';

			var li = $('<li class="' + pageEmptyItemClass + '" index="' + index + '">');
			var options = $('<div class="' + pageOptionsClass + '">');
			var img = $('<img class="' + pageThumbClass + '">');
			var url = $('<input type="text" name="_dflip[pages][' + index + '][url]" id="dflip-page-' + index + '-url"/>');

			li.append(img).append(options);
			options.append(url);

			createPageOptions(li);
			return li;
		}

		function createPageOptions(pageItem) {
			var
				container = $('<ul class="dflip-page-actions">'),
				image = $('<li class="dflip-page-image-action dashicons dashicons-format-image" title="Change Image">'),
				edit = $('<li class="dflip-page-edit-action dashicons dashicons-edit" title="Edit HotSpots">'),
				remove = $('<li class="dflip-page-remove-action dashicons dashicons-trash" title="Remove Page">');

			container.append(image).append(edit).append(remove).appendTo(pageItem);

		}

		$(document).on("click", ".dflip-page-remove-action", function () {
			var check = confirm("Delete the page!");
			if (check == true) {
				$(this).closest("." + pageItemClass).remove();
			}
		});
		$(document).on("click", ".dflip-page-edit-action", function () {
			var page = $(this).closest("." + pageItemClass);
			showPageModal(page);
		});

		$(document).on("click", ".dflip-outline-remove-action", function () {
			var check = confirm("Delete outline and its children!");
			if (check == true) {
				if ($(this).closest(".outline-node").siblings(".outline-node").length == 0) {
					$(this).closest(".outline-node").closest(".outline-node").removeClass("outline-haschild");
				}
				$(this).closest(".outline-node").remove();
			}
		});

		$(document).on("click", ".dflip-outline-add-action", function (event) {
			var node = $(this).closest(".outline-node");
			var container = node.find(">.outline-nodes");
			var prefix = node.data("prefix") + '[items]' + '[' + container.find(".outline-node").length + ']';

			addNode(container, prefix, {title: "", dest: ""});
			node.addClass("outline-haschild");
			event.stopPropagation();
		});

		$(document).on("click", ".outline-wrapper", function () {
			var current = $(".outline-active");
			var title = current.find(">.outline-wrapper > input[dtype='title']").val();
			var dest = current.find(">.outline-wrapper > input[dtype='dest']").val();
			var label = current.find(">.outline-wrapper > label").html(title + " : (" + dest + ")");
			current.removeClass("outline-active");

			$(this).closest(".outline-node").addClass("outline-active");
		});

		$(document).on("click", ".outline-collapse", function () {
			$(this).closest(".outline-node").toggleClass("outline-collapsed");
		});

		$(document).on("click", ".dflip-page-image-action", function () {
			var pageItem = $(this).closest("." + pageItemClass);
			uploadMedia({
				target: pageItem.find("input"),
				type: 'image',
				callback: function (url) {
					pageItem.find("." + pageThumbClass).attr("src", url);
				}
			});
		});

		$(".dflip-box .dflip-option >:input").on("change", function () {
			parse_condition();
			checkGlobal($(this));
		});

		createPageOptions($("." + pageItemClass));

		function match_condition(condition) {
			var match;
			var regex = /(.+?):(is|not|contains|less_than|less_than_or_equal_to|greater_than|greater_than_or_equal_to)\((.*?)\),?/g;
			var conditions = [];

			while (match = regex.exec(condition)) {
				conditions.push({
					'check': match[1],
					'rule': match[2],
					'value': match[3] || ''
				});
			}

			return conditions;
		}

		function parse_condition() {
			$('.dflip-box[id^="dflip_"][data-condition]').each(function () {

				var passed;
				var conditions = match_condition($(this).data('condition'));
				var operator = ( $(this).data('operator') || 'and' ).toLowerCase();

				$.each(conditions, function (index, condition) {

					//var target   = $( '#setting_' + condition.check );
					var targetEl = $("#" + condition.check);// !! target.length && target.find( OT_UI.condition_objects() ).first();

					//if ( ! target.length || ( ! targetEl.length && condition.value.toString() != '' ) ) {
					//    return;
					//}
					if (!targetEl.length) {
						return;
					}

					var v1 = targetEl.length ? targetEl.val().toString() : '';
					var v2 = condition.value.toString();
					var result;

					switch (condition.rule) {
						case 'less_than':
							result = ( parseInt(v1) < parseInt(v2) );
							break;
						case 'less_than_or_equal_to':
							result = ( parseInt(v1) <= parseInt(v2) );
							break;
						case 'greater_than':
							result = ( parseInt(v1) > parseInt(v2) );
							break;
						case 'greater_than_or_equal_to':
							result = ( parseInt(v1) >= parseInt(v2) );
							break;
						case 'contains':
							result = ( v1.indexOf(v2) !== -1 ? true : false );
							break;
						case 'is':
							result = ( v1 == v2 );
							break;
						case 'not':
							result = ( v1 != v2 );
							break;
					}

					if ('undefined' == typeof passed) {
						passed = result;
					}

					switch (operator) {
						case 'or':
							passed = ( passed || result );
							break;
						case 'and':
						default:
							passed = ( passed && result );
							break;
					}

				});

				if (passed) {
					$(this).animate({opacity: 'show', height: 'show'}, 200);
				} else {
					$(this).animate({opacity: 'hide', height: 'hide'}, 200);
				}

				delete passed;

			});
		}

		function checkGlobal(_this) {
			var globalValue = _this.data("global");
			var value = _this.val().trim();
			if (value == globalValue || (globalValue == undefined && value == "")) {
				_this.addClass("dflip-global-active").removeClass("dflip-global-inactive");
			} else {
				_this.addClass("dflip-global-inactive").removeClass("dflip-global-active");
			}
		}

		parse_condition();

		$('.dflip-box .dflip-option >:input[id^="dflip_"][data-global]').each(function () {
			checkGlobal($(this));
		});

		//create Outline
		if ($("#dflip_outline").length > 0) {
			var data = JSON.parse($("#dflip_outline").val());
			data = revalidateArray(data, 'items');

			if (data.length == void 0 || data.length == 0)
				data = [];

			maxIndex = data.length;
			var addNodeBtn = $('<div class="add-outline-btn button button-primary">Add New Outline</div>').appendTo(outlineBox).on("click", function () {
				addNode(outlineBox, "_dflip[outline]" + '[' + maxIndex + ']', {title: "", dest: ""});
				maxIndex++;
			});
			nodeTree(outlineBox, data, "_dflip[outline]");

			dragEnable(outlineBox, ".outline-wrapper");
		}
	});

	function revalidateArray(array, scan) {
		if (array == void 0) return array;

		var data = array;
		//convert to array
		if (array.length == void 0) {
			data = [];
			for (var prop in array) {
				data.push(array[prop]);
			}
		}
		//convert scan element to array
		for (var i = 0; i < data.length; i++) {
			if (data[i] !== void 0 && data[i][scan] !== void 0)
				data[i][scan] = revalidateArray(data[i][scan], scan)
		}

		return data;
	}

	function addNode(container, prefix, option) {
		var node = $('<div class="outline-node">').data("prefix", prefix),
			wrapper = $('<div class="outline-wrapper">'),
			label = $('<label></label>').html(option.title + " : (" + option.dest + ")"),
			title = $('<input name=' + prefix + '[title]" dtype="title" placeholder="Name of outline"/>').val(option.title),
			dest = $('<input name=' + prefix + '[dest]" dtype="dest" placeholder="pagenumber or url"/>').val(option.dest),
			nodes = $('<div class="outline-nodes">'),
			collapse = $('<div class="outline-collapse dashicons dashicons-arrow-down-alt2">'),

			actions = $('<ul class="dflip-outline-actions">'),
			add = $('<li class="dflip-outline-add-action dashicons dashicons-plus" title="Add Outline">'),
			remove = $('<li class="dflip-outline-remove-action dashicons dashicons-trash" title="Remove Outline">');

		wrapper.append(label).append(title).append(dest).appendTo(node);

		node.append(wrapper).append(nodes).append(collapse).appendTo(container);

		actions.append(add).append(remove).appendTo(wrapper);

		if (option.items !== void 0) {
			node.addClass("outline-haschild");
			nodeTree(nodes, option.items, prefix + "[items]");
		}

		return node;
	}

	function nodeTree(container, options, prefix) {

		//var container = $(this);
		if (options !== void 0 && options.length > 0) {
			for (var i = 0; i < options.length; i++) {
				var option = options[i];

				var node = addNode(container, prefix + '[' + i + ']', option)

			}
		}
		return this;

	}

	var maxIndex = 0;

	function dragEnable(container, selector) {
		//var container = $(this);
		var helper = $('<div class="drag-helper">').appendTo(container).hide();
		var x, y,
			dx, dy,
			initX, initY,
			state,
			node,
			drag_type = '',
			startNode,
			mousedown = false,
			dragging = false;

		function update(e) {
			if (node == void 0) return;
			var _y = e.pageY - node.offset().top;
			document.title = _y.toString();
			var _drag_type = _y < 5
				? "before"
				: _y > 27 ? "after" : "over";

			if (_drag_type !== drag_type) {
				drag_type = _drag_type;
				node.removeClass("has-drag-over has-drag-before has-drag-after").addClass("has-drag-" + drag_type);
			}
			helper.html("Insert " + drag_type + " " + node.find("label").html());
		}

		function checkChilds(node) {
			if (node.find(".outline-node").length > 0) {
				node.addClass("outline-haschild");
			}
			else {
				node.removeClass("outline-haschild");
			}
		}

		function revalidate(node, prefix) {
			var target;
			if (prefix == void 0) {
				//first find the parent.
				target = node.parents(".outline-node").first();
				if (target.length == 0) {
					//node was dropped to top level
					//increase index by 1
					maxIndex++;

					target = node;
					prefix = "_dflip[outline][" + maxIndex + "]";
					target.data("prefix", prefix);
					//update it's node and continue to child
				}
				else {
					prefix = target.data("prefix");
					//continue as normal
				}
			}
			else {
				target = node;
				target.data("prefix", prefix);
			}

			target.find(" >.outline-wrapper >input").each(function () {
				var input = $(this);
				var name = input.attr("name");
				var type = input.attr("dtype");

				input.attr("name", prefix + "[" + type + "]")
			});

			var index = 0;
			target.find(" >.outline-nodes > .outline-node").each(function () {
				revalidate($(this), prefix + "[items][" + index + "]");
				index++;
			});
		}

		function drop() {
			if (startNode !== void 0 && node !== void 0 && drag_type !== '') {
				var _source = startNode.closest(".outline-node");
				var _target = node.closest(".outline-node");
				var oldParent = _source.parents(".outline-node");
				if (_source.has(_target).length > 0 || _source.is(_target)) {
					alert("Can't drop into child");
					return;
				}
				if (drag_type == "before") {
					_source.insertBefore(_target);
				}
				else if (drag_type == "over") {
					node.siblings(".outline-nodes").append(_source);
				}
				else if (drag_type == "after") {
					_source.insertAfter(_target);
				}
				checkChilds(oldParent);
				checkChilds(_source);
				checkChilds(_target);
				revalidate(_source);
			}
		}

		container
			.on("mousedown", function (e) {
				if (e.srcElement.nodeName == "INPUT") return;
				startNode = $(e.srcElement).closest(selector);
				if (e.button !== 0) return;
				if (startNode.length == 0) return;

				initX = e.pageX - $(this).offset().left;
				initY = e.pageY - $(this).offset().top;
				mousedown = true;
			})
			.on("mousemove", function (e) {
				if (!dragging && mousedown == true) {
					dx = e.pageX - $(this).offset().left - initX;
					dy = e.pageY - $(this).offset().top - initY;

					if (Math.abs(dx) > 5 || Math.abs(dy) > 5) {
						dragging = true;
						helper.show();
						container.addClass("has-dragging");
						startNode.addClass("is-drag-source");
					}
				}
				if (dragging) {
					x = e.pageX - $(this).offset().left;
					y = e.pageY - $(this).offset().top;
					helper.css({
						left: x - 20,
						top: y + 15
					});
					update(e);
				}
			});

		$(window)
			.on("mouseup", function (e) {
				container.removeClass("has-dragging");
				if (startNode) startNode.removeClass("is-drag-source");

				if (node && dragging == true) {
					node.removeClass("has-drag-over has-drag-before has-drag-after");
					drop();
				}
				dragging = false;
				mousedown = false;
				helper.hide();

				node = null;
				startNode = null;

			});

		container
			.on("mouseover", selector, function (e) {
				if (mousedown == true) {
					if (node)
						node.removeClass("has-drag-over has-drag-before has-drag-after");
					node = $(this);
				}
				if (dragging == true && node) {
					update(e);
					node.addClass("has-drag-over");
				}
			})
	}

	var pageModal;
	var activePage;
	var activeSpot;

	function showPageModal(page) {

		function createSpots(index) {
			var spot = $('<input class="dflip-hotspot-input" name="_dflip[pages][' + activePage.attr('index') + '][hotspots][' + index + ']" />');
			spot.val("[30,30,30,30,]");
			return spot;
		}

		if (pageModal == void 0) {

			pageModal = $('<div class="dflip-page-modal media-modal">');

			var modalContent = $('<div class="media-modal-content edit-attachment-frame ">'),
				frameContent = $('<div class="media-frame-content">'),
				header = $('<div class="edit-media-header">'),
				next = $('<div class="page-modal-next right dashicons">'),
				prev = $('<div class="page-modal-prev left dashicons">'),
				close = $('<div class="page-modal-close media-modal-close"><span class="media-modal-icon"></span></div>'),

				subHeader = $('<div class="dflip-hotspot-header">'),
				addHotspot = $('<div class="dflip-add-hotspot button button-primary">Add Hot-Spot</div>'),
				del = $('<div class="dflip-remove-hotspot button button-secondary">Remove Hot-Spot</div>'),
				dest = $('<input class="dflip-hotspot-dest" placeholder="Enter page number or url with http:\\\\"></div>'),
				divImage = $('<div class="page-modal-image-wrapper">'),
				image = $('<img class="page-modal-image">'),
				content = $('<input class="page-modal-html" >');

			pageModal.divImage = divImage;
			pageModal.image = image;
			pageModal.content = content;
			pageModal.dest = dest;

			next.on("click", function () {
				var next = activePage.next();
				if (next.length > 0 && next.hasClass("dflip-page-item")) {
					showPageModal(next);
				}
			});
			prev.on("click", function () {
				var prev = activePage.prev();
				if (prev.length > 0 && prev.hasClass("dflip-page-item")) {
					showPageModal(prev);
				}
			});
			close.on("click", function () {
				pageModal.hide();
			});
			dest.on("change", function () {
				if (spotHelper._hotspot !== void 0) {
					spotHelper._hotspot.dest = $(this).val();
					spotHelper._hotspot.update();
				}
			});
			del.on("click", function () {
				var _del = confirm("Delete hotspot?");
				if (_del == true) {
					spotHelper._hotspot.dispose(true);
					spotHelper.detach();
				}
				dest.val("");
			});
			addHotspot.on("click", function () {
				var spots = activePage.find(".dflip-hotspot-input");

				var spotindex = activePage.attr('hotspots');
				if (spotindex == void 0) {
					spotindex = activePage.find(".dflip-hotspot-input").length;
				}
				var _hotspotInput = createSpots(spotindex);
				spotindex++;
				activePage.attr('hotspots', spotindex);
				activePage.find(".dflip-page-options").append(_hotspotInput);
				var _hotspot = new HotSpot([40, 40, 20, 20, ''], pageModal.divImage);
				_hotspot.activate(spotHelper);
				_hotspot.target = _hotspotInput;
			});
			divImage.append(image);
			subHeader.append(addHotspot).append(dest).append(del);
			frameContent.append(subHeader).append(divImage);
			header.append(prev).append(next);
			modalContent.append(header).append(frameContent);
			pageModal.append(close).append(modalContent).appendTo($("#dflip_pages_box"));

		}
		pageModal.show();
		pageModal.dest.val("");
		var src = page.find(".dflip-page-thumb").attr("src");
		pageModal.image.attr("src", src);

		//clear old hotspots
		if (activePage != void 0 && activePage.hotspots != void 0) {
			for (var h = 0; h < activePage.hotspots.length; h++) {
				activePage.hotspots[h].dispose();
			}
		}

		activePage = page;

		if (page.hotspots == void 0)
			page.hotspots = [];
		//fallback clearance
		if (spotHelper !== void 0)
			spotHelper._el.hide();
		pageModal.find(".dflip-hotspot").remove();

		var hotspots = [];
		page.find(".dflip-hotspot-input").each(function (index) {
			hotspots[index] = $(this).val().substr(1).slice(0, -1).split(",");
			var spot = new HotSpot(hotspots[index], pageModal.divImage);
			spot.target = $(this);
			page.hotspots[index] = spot;
		});

	}

	//[160, 105, 250, 30, 2]
	var HotSpot = function (option, parent) {
		var _this = this;
		_this.left = option[0],
			_this.top = option[1],
			_this.width = option[2],
			_this.height = option[3],
			_this.dest = option[4],
			_this.ref = parent.find("img.page-modal-image");

		_this._el = $('<div class="dflip-hotspot">');

		parent.append(_this._el);
		_this.update();
		_this._el
			.on("click", function () {
				_this.activate(spotHelper);
			});
	};

	HotSpot.prototype.activate = function (helper) {
		helper.attach(this);
		pageModal.dest.val(this.dest);
	};
	HotSpot.prototype.deactivate = function (helper) {

	};
	HotSpot.prototype.dispose = function (removeTarget) {
		this._el.off();
		this._el.remove();
		if (removeTarget == true && this.target !== void 0) {
			this.target.remove();
		}
	};
	HotSpot.prototype.updateSize = function (size) {
		this.width = Math.round(10000 * size.width / this.ref.width()) / 100;
		this.height = Math.round(10000 * size.height / this.ref.height()) / 100;
	};
	HotSpot.prototype.updatePosition = function (position) {
		this.left = Math.round(10000 * position.left / this.ref.width()) / 100;
		this.top = Math.round(10000 * position.top / this.ref.height()) / 100;
		this.update();
	};
	HotSpot.prototype.update = function () {
		this._el.css({
			left: this.left + "%",
			top: this.top + "%",
			width: this.width + "%",
			height: this.height + "%"
		});
		if (this.target !== void 0) {
			this.target.val('[' + this.left + ',' + this.top + ',' + this.width + ',' + this.height + ',' + this.dest + ']');
		}
	};


	var SpotHelper = function () {
		var _this = this;
		_this.initialized = false;
		_this._hotspot = void 0;
		if ($.fn.draggable) {
			_this._el = $('<div class="dflip-hotspot-helper">')
				.draggable({
					containment: "parent",
					drag: function (event, ui) {
						_this._hotspot.updatePosition(ui.position);
					}
				});
		}
		else {
			_this._el = void 0;
			//console.log("Could not load jQuery draggable")
		}

	};
	SpotHelper.prototype.attach = function (hotspot) {
		var _this = this;
		if (_this._hotspot !== void 0)
			_this._hotspot.deactivate();


		_this._hotspot = hotspot;
		_this.container = hotspot._el.parent();
		_this.container.append(_this._el);

		if (_this.initialized != true) {
			_this._el.resizable({
				handles: 'ne, se, sw, nw',
				resize: function (event, ui) {
					_this._hotspot.updateSize(ui.size);
					_this._hotspot.updatePosition(ui.position);
				}
			});
			_this.initialized = true;
		}

		_this._el.css({
			left: hotspot._el[0].style.left,
			top: hotspot._el[0].style.top,
			width: hotspot._el[0].style.width,
			height: hotspot._el[0].style.height,
			display: "block"
		});
		//_this._hotspot.activate(this);
	};
	SpotHelper.prototype.detach = function (hotspot) {
		if (this._hotspot !== void 0)
			this._hotspot.deactivate();
		this._el.hide();
	};
	var spotHelper = new SpotHelper();

	/*({
	 backgroundImage:"url(" + src+")"
	 });*/
})(jQuery);